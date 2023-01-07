{%- comment -%}
SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
SPDX-License-Identifier: MIT
{%- endcomment -%}
{%- comment -%}
  ref: https://github.com/kurtmkurtm/LiquidTestReports/blob/master/docs/Properties.md
  ref: https://shopify.github.io/liquid/basics/introduction/
{%- endcomment -%}
{%- assign passed   = run.test_run_statistics.passed_count -%}
{%- assign failed   = run.test_run_statistics.failed_count -%}
{%- assign skipped  = run.test_run_statistics.skipped_count -%}
{%- assign total    = run.test_run_statistics.executed_tests_count -%}
{%- assign pass_percentage      = passed | divided_by: total | times: 100.0 | round: 2 *-%}
{%- assign failed_percentage    = failed | divided_by: total | times: 100.0 | round: 2 *-%}
{%- assign skipped_percentage   = skipped | divided_by: total | times: 100.0 | round: 2 *-%}
{%- assign information  = run.messages | where: "level", "Informational" -%}
{%- assign warnings     = run.messages | where: "level", "Warning" -%}
{%- assign errors       = run.messages | where: "level", "Error" -%}
{%- if passed == total -%}
{%-   assign overall = "✔️ Pass" *-%}
{%- elsif failed == 0 -%}
{%-   assign overall = "⚠️ Indeterminate" *-%}
{%- else -%}
{%-   assign overall = "❌ Fail" *-%}
{%- endif -%}

## {{overall}}: {{ parameters.TestTargetName }} ({{ parameters.TargetFramework }} on {{ parameters.RuntimeIdentifier }})

|✔️ Passed|❌ Failed|⚠️ Skipped|Total|
|-|-|-|-|
|{{passed}}|{{failed}}|{{skipped}}|{{total}}|
|{{pass_percentage}}%|{{failed_percentage}}%|{{skipped_percentage}}%||

<details>
  <summary>Test details</summary>
  <dl>
    <dt>Duration</dt>
    <dd>{{ run.elapsed_time_in_running_tests | format_rfc3339 }}</dd>
    <dt>Started at</dt>
    <dd><time>{{ run.started | date: '%Y-%m-%dT%H:%M:%S%Z' }}</time></dd>
    <dt>Finished at</dt>
    <dd><time>{{ run.finished | | date: '%Y-%m-%dT%H:%M:%S%Z' }}</time></dd>
  </dl>
{%- if parameters.EmitTestMessages == 'true' -%}
  <dl>
{%- if errors.size != 0 -%}
    <dt>Errors</dt>
    <dd><pre><code>{%- for message in errors -%}{{ message.message | escape }}
{%- endfor -%}</code></pre></dd>
{%- endif -%}
{%- if warnings.size != 0 -%}
    <dt>Warnings</summary>
    <dd><pre><code>{%- for message in warnings -%}{{ message.message | escape }}
{%- endfor -%}</code></pre></dd>
{%- endif -%}
{%- if information.size != 0 -%}
    <dt>Informational messages</dt>
    <dd><pre><code>{%- for message in information -%}{{ message.message | escape }}
{%- endfor -%}</code></pre></dd>
{%- endif -%}
  </dl>
{%- endif -%}
</details>

{%- for set in run.result_sets -%}
### {{ set.source | path_split | last }} - {{ set.passed_count | divided_by: set.executed_tests_count | times: 100.0 | round: 1 }}% passed
<details>
  <summary>Result details</summary>
  <table>
    <thead>
      <tr>
        <th>Result</th>
        <th>Test</th>
        <th>Duration</th>
      </tr>
    </thead>
    <tbody>
      {%- comment -%}
      === results except passed test case ===
      {%- endcomment -%}
      {%- for result in set.results -%}
      {%- if result.outcome != 'Passed' -%}
      <tr>
        <td>{% case result.outcome %} {% when 'Failed' %}❌{% else %}⚠️{% endcase %} {{ result.outcome }}</td>
        <td>
          <span>{{- result.test_case.display_name | escape -}}</span>
          {%- if parameters.EmitTestCaseResultMessages == 'true' -%}
          <dl>
            {%- if result.error_message != null and result.error_message != "" -%}
            <dt>Message:</dt>
            <dd><pre><code>{{ result.error_message | escape }}</code></pre></dd>
            {%- endif -%}
            {%- if result.error_stack_trace != null and result.error_stack_trace != "" -%}
            <dt>Stack trace:</dt>
            <dd><pre><code>{{ result.error_stack_trace | escape }}</code></pre></dd>
            {%- endif -%}
          </dl>
          {%- endif -%}
        </td>
        <td>{{ result.duration | format_duration | escape }}</td>
      </tr>
      {%- endif -%}
      {%- endfor -%}
      {%- comment -%}
      === results passed test case ===
      {%- endcomment -%}
      {%- for result in set.results -%}
      {%- if result.outcome == 'Passed' -%}
      <tr>
        <td>✔️ {{ result.outcome }}</td>
        <td>{{- result.test_case.display_name | escape -}}</td>
        <td>{{ result.duration | format_duration | escape }}</td>
      </tr>
      {%- endif -%}
      {%- endfor -%}
    </tbody>
  </table>
</details>
{%- endfor -%}

----

[{{ library.text }}]({{ library.link }})
