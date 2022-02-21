# Workflows
## workflows/release-target-v1.1.2
**release target**の作成と公開を行うworkflow

### 概略
1. release target tagがpushされたとき、**release target** [^release-target]を作成し、リリース確認のPull Requestを作成する ([generate-release-target.yml](#generate-release-target.yml))
2. 上記Pull Requestがマージされたとき、作成したrelease targetをもとにNuGet packageのpushとGitHub Releaseの公開を行う ([publish-release-target.yml](#publish-release-target.yml))

[^release-target]: NuGet package・release working branch名・release tag等のリリースに必要な情報

### [generate-release-target.yml](/.github/workflows/generate-release-target.yml)
このworkflowではrelease targetを作成し、リリース確認のPull Requestを作成する。

- トリガー
  - release target tagがpushされたとき
  - release target tagを指定してworkflowがdispatchされたとき
- 前提条件
  - release target tagの形式
    - `<prefix>/<branch>/<package-id>-<package-version>[-<package-prerelease>]`の形式であること
      - `<prefix>`はworkflow input `release_target_tag_prefix`で変更可能、デフォルトは`new-release/`
  - リポジトリのディレクトリ構造
    - `<source-root>/<package-id>/`のディレクトリが存在すること
      - 上記ディレクトリに単一のproject fileが存在し、`--configuration Release`で`dotnet pack`できること
      - `<source-root>`はworkflow input `path_prefix_source`で変更可能、デフォルトは`src/`
    - project fileがAPIリストを生成する場合は、workflow input `path_prefix_apilist`で出力先ディレクトリが指定されていること、デフォルトは`doc/api-list/`
  - secrets
    - `token`: コードのcheckoutとリポジトリへのPull Requestを作成できる権限が設定されていること

このworkflowで行う作業は以下のとおり。

1. パッケージ情報を収集・決定する
   1. NuGet package作成時の
      1. コードをチェックアウトする際に用いるタグ (release target tag)
      2. 上記タグの属するブランチ (nuspecに含めるための情報、release working branchのマージ先となるブランチ)
   2. NuGet package作成対象の
      1. package IDとpackage version
      2. ソースディレクトリ
      3. 直近バージョンのrelease tag (変更履歴の作成に用いる)
         - 自動検出できない場合は、リポジトリのinitial commitを使用
2. NuGet packageを作成し、artifactとして一時保存する
3. リリース情報を決定する
   1. release tagを決定する
      - `<prefix>/<package-id>-<package-version>[-<package-prerelease>]`の形式で決定する
      - prefixはworkflow input `release_tag_prefix`で変更可能、デフォルトは`releases/`
   2. release working branchを作成する
      - ブランチ名は`<release-tag>-<unixtime>`の形式で決定する
      - ビルド時にAPIリストが作成される場合は、release working branchにcommitする
      - 作成されない場合は空のcommitを追加する
4. リリース準備のPull Requestを作成する
   1. headをrelease working branch、baseをrelease target tagで指定されたブランチとして、Pull Requestを作成する
   2. Pull Requestの本文を構成する
      1. 後続のworkflowでNuGet packageをダウンロードするために現workflowの`run id`を含むURLを含める
      2. リリース前の確認用にrelease target情報を含める
      3. 上記と下記のセクションを分割するための文字列`<!-- RELEASE NOTE -->`を含める
      4. commit履歴、APIリストの差分、以前のバージョンとの差分リンクを収集してGitHub Releaseの草稿を作成して本文に含める
   3. Pull Requestを作成する

### [publish-release-target.yml](/.github/workflows/publish-release-target.yml)
このworkflowでは作成されたrelease targetをもとに、NuGet packageのpushと、GitHub Releaseの公開を行う。

- トリガー
  - Pull Requestがクローズされたとき
- 前提条件
  - Pull Requestのクローズがrelease working branchのマージが契機であること
  - Pull Requestの本文に以下が含まれていること
    - generate-release-target.ymlの`run id`を含むURL
    - release target情報
    - 文字列`<!-- RELEASE NOTE -->`
    - GitHub Releaseの草稿
  - secrets
    - `token_github`: コードのcheckout・Pull Request本文の取得・artifactのダウンロードを行うことができる権限が設定されていること
    - `token_nuget_org`: (オプション)`nuget.org`にNuGet packageをpushする場合、それができる権限が設定されていること
    - `token_github_packages`: (オプション)GitHub PackagesにNuGet packageをpushする場合、それができる権限が設定されていること

このworkflowで行う作業は以下のとおり。

1. release target情報を収集する
   1. マージされたPull Requestの本文を取得し
      1. pushするNuGet packageのartifactが保存されているworkflowの`run id`を取得する
      2. パッケージ情報を取得する
      3. release working branch名と、release tag名を取得する
      4. GitHub Releaseの草稿を取得する
2. artifactとして保存されているNuGet packageをダウンロードし
   1. (`token_nuget_org`が設定されている場合) `nuget.org`にpushする
   2. (`token_github_packages` が設定されている場合)GitHub Packagesにpushする
3. release tagを作成する (Pull Requestがmergeされた時点のHEADをポイントする)
4. GitHub Releaseを作成・公開する
5. 後処理を行う
   1. release target tagを削除する
   2. release working branchを削除する

### TODO
- API diffがバージョンのみとなる場合は省略する
- pull requestのbodyが65536 characters以上となる場合、作成に失敗する: `pull request create failed: GraphQL: Body is too long (maximum is 65536 characters) (createPullRequest)`
