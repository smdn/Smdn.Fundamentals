{%- comment -%}
SPDX-FileCopyrightText: 2023 smdn <smdn@smdn.jp>
SPDX-License-Identifier: MIT
{%- endcomment -%}
{%- comment -%}
  ref: https://github.com/kurtmkurtm/LiquidTestReports/blob/master/docs/Properties.md
  ref: https://shopify.github.io/liquid/basics/introduction/
{%- endcomment -%}
{%- assign symbol_passed = "🟢" -%}
{%- assign symbol_indeterminate = "🟡" -%}
{%- assign symbol_failed = "🔴" -%}
{%- assign test_count_passed   = run.test_run_statistics.passed_count -%}
{%- assign test_count_failed   = run.test_run_statistics.failed_count -%}
{%- assign test_count_skipped  = run.test_run_statistics.skipped_count -%}
{%- assign test_count_total    = run.test_run_statistics.executed_tests_count -%}
{%- if test_count_passed == test_count_total -%}
{%-   assign overall = symbol_passed | append: " Pass" *-%}
{%- elsif test_count_failed == 0 -%}
{%-   assign overall = symbol_indeterminate | append: " Indeterminate" *-%}
{%- else -%}
{%-   assign overall = symbol_failed | append: " Fail" *-%}
{%- endif -%}
{%- case parameters.OmitPassedTestResults | downcase -%}
{%-   when 'true' -%}{%- assign parameter_emit_passed_test_results = false -%}
{%-   else -%}{%- assign parameter_emit_passed_test_results = true -%}
{%- endcase -%}
{%- case parameters.EmitTestMessages | downcase -%}
{%-   when 'true' -%}{%- assign parameter_emit_test_messages = true -%}
{%-   else -%}{%- assign parameter_emit_test_messages = false -%}
{%- endcase -%}
{%- case parameters.EmitTestCaseResultMessages | downcase -%}
{%-   when 'true' -%}{%- assign parameter_emit_result_messages = true -%}
{%-   else -%}{%- assign parameter_emit_result_messages = false -%}
{%- endcase -%}
## {{overall}}: {{ parameters.TestTargetName }} ({{ parameters.TargetFramework }} on {{ parameters.RuntimeIdentifier }})
{%- assign percentage_passed  = test_count_passed  | divided_by: test_count_total | times: 100.0 | round: 2 *-%}
{%- assign percentage_failed  = test_count_failed  | divided_by: test_count_total | times: 100.0 | round: 2 *-%}
{%- assign percentage_skipped = test_count_skipped | divided_by: test_count_total | times: 100.0 | round: 2 *-%}
{%- if test_count_passed != test_count_total -%}
{%-   assign emit_overall_table = true -%}
{%- else -%}
{%-   assign emit_overall_table = parameter_emit_passed_test_results -%}
{%- endif -%}
{%- if emit_overall_table -%}
<table>
  <thead>
    <tr>
      <th></th>
      <th scope="col">{{symbol_passed}} Passed</th>
      <th scope="col">{{symbol_failed}} Failed</th>
      <th scope="col">{{symbol_indeterminate}} Skipped</th>
      <th scope="col">Total</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th scope="row">#</th>
      <td>{{ test_count_passed }}</td>
      <td>{{ test_count_failed }}</td>
      <td>{{ test_count_skipped }}</td>
      <td>{{ test_count_total }}</td>
    </tr>
    <tr>
      <th scope="row">%</th>
      <td>{{ percentage_passed }}</td>
      <td>{{ percentage_failed }}</td>
      <td>{{ percentage_skipped }}</td>
      <td>100</td>
    </tr>
  </tbody>
</table>
{%- endif -%}

{%- if percentage_failed == 0 -%}
{%-   assign test_details_open_attr = '' -%}
{%- else -%}
{%-   assign test_details_open_attr = ' open' -%}
{%- endif -%}
<details{{ test_details_open_attr }}>
  <summary>Test details</summary>
  <dl>
    <dt>Duration</dt>
    <dd>{{ run.elapsed_time_in_running_tests | format_rfc3339 }}</dd>
    <dt>Started at</dt>
    <dd><time>{{ run.started | date: '%Y-%m-%dT%H:%M:%S%Z' }}</time></dd>
    <dt>Finished at</dt>
    <dd><time>{{ run.finished | | date: '%Y-%m-%dT%H:%M:%S%Z' }}</time></dd>
  </dl>
{%- if parameter_emit_test_messages | -%}
{%-   assign messages_error         = run.messages | where: "level", "Error" -%}
{%-   assign messages_warning       = run.messages | where: "level", "Warning" -%}
{%-   assign messages_informational = run.messages | where: "level", "Informational" -%}
  <dl>
{%-   if messages_error.size != 0 -%}
    <dt>❌ Errors</dt>
    <dd><pre><code>{%- for msg in messages_error -%}[{{ msg.level }}]{{ msg.message | escape }}
{%-     endfor -%}</code></pre></dd>
{%-   endif -%}
{%-   if messages_warning.size != 0 -%}
    <dt>⚠️ Warnings</summary>
    <dd><pre><code>{%- for msg in messages_warning -%}[{{ msg.level }}]{{ msg.message | escape }}
{%-     endfor -%}</code></pre></dd>
{%-   endif -%}
{%-   if messages_informational.size != 0 -%}
    <dt>❕ Informational messages</dt>
    <dd><pre><code>{%- for msg in messages_informational -%}[{{ msg.level }}]{{ msg.message | escape }}
{%-     endfor -%}</code></pre></dd>
{%-   endif -%}
  </dl>
{%- endif -%}
</details>

{%- for set in run.result_sets -%}
{%-   if set.passed_count == set.executed_tests_count -%}
{%-     capture set_result_summary -%} ✔All {{ set.passed_count}} tests passed {%- endcapture -%}
{%-   elsif set.failed_count == 0 -%}
{%-     capture set_result_summary -%} ✔{{ set.passed_count}} of {{ set.executed_tests_count}} tests passed {%- endcapture -%}
{%-   else -%}
{%-     capture set_result_summary -%} ✖{{ set.failed_count}} tests failed ✔{{ set.passed_count}} of {{ set.executed_tests_count}} tests passed{%- endcapture -%}
{%-   endif -%}
### {{ set.source | path_split | last }} - {{ set_result_summary }}
{%-   comment -%}
        convert the test results into a list of indices, categorized by their outcomes.
        and then convert list into a joined string in the following format:
          "Passed=0,1,3,6..."
          "Indeterminate=2,5..."
          "Failed=4"
{%-   endcomment -%}
{%-   assign result_index_list_of_passed = '' -%}
{%-   assign result_index_list_of_indeterminate = '' -%}
{%-   assign result_index_list_of_failed = '' -%}
{%-   for result in set.results -%}
{%-     if result.outcome == 'Passed' -%}
{%-       if parameter_emit_passed_test_results -%}
{%-         capture result_index_list_of_passed -%}{{ result_index_list_of_passed }},{{ forloop.index0 }}{%- endcapture -%}
{%-       endif -%}
{%-     elsif result.outcome == "Failed" -%}
{%-       capture result_index_list_of_failed -%}{{ result_index_list_of_failed }},{{ forloop.index0 }}{%- endcapture -%}
{%-     else -%}
{%-       capture result_index_list_of_indeterminate -%}{{ result_index_list_of_indeterminate }},{{ forloop.index0 }}{%- endcapture -%}
{%-     endif -%}
{%-   endfor -%}
{%-   capture result_index_list_of_passed -%}Passed={{ result_index_list_of_passed | remove_first: ',' }}{%- endcapture -%}
{%-   capture result_index_list_of_indeterminate -%}Indeterminate={{ result_index_list_of_indeterminate | remove_first: ',' }}{%- endcapture -%}
{%-   capture result_index_list_of_failed -%}Failed={{ result_index_list_of_failed | remove_first: ',' }}{%- endcapture -%}
{%-   comment -%}
        construct array of result_index_list_of_*
{%-   endcomment -%}
{%-   capture result_index_lists_in_string -%}{{ result_index_list_of_failed }};{{ result_index_list_of_indeterminate }};{{result_index_list_of_passed}}{%- endcapture -%}
{%-   assign result_index_lists = result_index_lists_in_string | split: ';' -%}
{%-   for result_index_list in result_index_lists -%}
{%-     comment -%}
          split result_index_list into outcome and array of indices
{%-     endcomment -%}
{%-     assign result_outcome_and_index_list = result_index_list | split: '=' -%}
{%-     if result_outcome_and_index_list.size == 1 -%}
{%-       continue -%}
{%-     endif -%}
{%-     assign result_outcome = result_outcome_and_index_list[0] -%}
{%-     assign result_indices = result_outcome_and_index_list[1] | split: ',' -%}
{%-     case result_outcome -%}
{%-       when 'Passed' -%}
{%-         assign result_symbol = symbol_passed -%}
{%-         assign result_details_open_attr = '' -%}
{%-       when 'Failed' -%}
{%-         assign result_symbol = symbol_failed -%}
{%-         assign result_details_open_attr = ' open' -%}
{%-       else -%}
{%-         assign result_symbol = symbol_indeterminate -%}
{%-         assign result_details_open_attr = ' open' -%}
{%-     endcase -%}
<details{{ result_details_open_attr }}>
  <summary>{{ result_symbol }} {{result_outcome}} test details</summary>
  <table>
    <thead>
      <tr>
        <th>Test</th>
        <th>Duration</th>
      </tr>
    </thead>
    <tbody>
{%-     comment -%}
          find and enumerate corresponding test results from their index
{%-     endcomment -%}
{%-     for result_index in result_indices -%}
{%-       assign result = set.results | slice: result_index | first -%}{%- comment -%} array[index] does not works fine, so use slice+first instead {%- endcomment -%}
      <tr>
        <td>
          <span>{{ result_symbol }} <code>{{- result.test_case.display_name | escape -}}</code></span>
          {%- if parameter_emit_result_messages -%}
          {%-   if result.error_message != null and result.error_message != "" -%}
          {%-     assign result_has_error_message = true -%}
          {%-   else -%}
          {%-     assign result_has_error_message = false -%}
          {%-   endif -%}
          {%-   if result.error_stack_trace != null and result.error_stack_trace != "" -%}
          {%-     assign result_has_error_stack_trace = true -%}
          {%-   else -%}
          {%-     assign result_has_error_stack_trace = false -%}
          {%-   endif -%}
          {%-   if result_has_error_message or result_has_error_stack_trace -%}
          <dl>
          {%-     if result_has_error_message -%}
            <dt>Message:</dt>
            <dd><pre><code>{{ result.error_message | escape }}</code></pre></dd>
          {%-     endif -%}
          {%-     if result_has_error_stack_trace -%}
            <dt>Stack trace:</dt>
            <dd><pre><code>{{ result.error_stack_trace | escape }}</code></pre></dd>
          {%-     endif -%}
          {%-   endif -%}
          </dl>
          {%- endif -%}
        </td>
        <td>{{ result.duration | format_duration | escape }}</td>
      </tr>
{%-     endfor -%}
    </tbody>
  </table>
</details>
{%-   endfor -%}
{%- endfor -%}

----

[{{ library.text }}]({{ library.link }})
