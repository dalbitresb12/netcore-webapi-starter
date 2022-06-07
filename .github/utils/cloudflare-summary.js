/**
 * @param {string} str
 * @returns {boolean}
 */
const isValidString = (str) => {
  return typeof str === "string" && str.length > 0;
};

/**
 * @param {string} tag
 * @param {string} content
 * @param {boolean} endTag
 * @param {Record<string, string | number | boolean>} attributes
 * @returns {string}
 */
const createTag = (tag = "span", content = "", endTag = true, attributes = {}) => {
  const attrs = Object.entries(attributes).map(([key, value]) => ` ${key}="${value}"`).join('');
  return `<${tag}${attrs}>${content}${endTag ? `</${tag}>` : ''}`;
};

/**
 * @param {string} str
 * @returns {string}
 */
const codeblock = (str) => createTag("code", str);

/**
 * @param {string} str
 * @returns {string}
 */
const bold = (str) => createTag("strong", str);

const emojis = {
  build: {
    success: ':white_check_mark:',
    failed: ':no_entry_sign:',
    progress: ':zap:'
  },
  check: ':heavy_check_mark:',
  times: ':x:',
};

/**
 * @param {string} str
 * @returns {string}
 */
const spacing = (str) => `&nbsp;${str}&nbsp;`;

/**
 * @typedef TestReport
 * @property {"success" | "failure"} conclusion
 * @property {number} passed
 * @property {number} failed
 * @property {number} skipped
 * @property {number} time
 */

/**
 * @param {unknown} value
 * @returns {value is TestReport}
 */
const validateTestReport = (value) => {
  try {
    if (typeof value !== "object") return false;
    if (typeof value.conclusion !== "string") return false;
    for (const key of ["passed", "failed", "skipped", "time"]) {
      value[key] = Number(value[key]);
      if (!Number.isInteger(value[key])) return false;
    }
    if (typeof value.passed !== "number") return false;
    if (typeof value.failed !== "number") return false;
    if (typeof value.skipped !== "number") return false;
    if (typeof value.time !== "number") return false;
    return true;
  } catch (e) {
    return false;
  }
};

/**
 * @param {TestReport} testReport
 * @returns {string}
 */
const createTestsBadge = (testReport) => {
  const { conclusion, passed, failed } = testReport;
  if (conclusion === "success") {
    return `https://img.shields.io/badge/tests-${passed}%20passed-brightgreen`;
  }
  return `https://img.shields.io/badge/tests-${passed}%20passed,%20${failed}%20failed-red`;
};

/**
 * @param {number} value
 * @param {string} word
 * @param {string} pluralization
 */
const pluralize = (value, word, pluralization) => {
  if (value === 1) return word;
  return `${word}${pluralization}`;
};

/**
 * @param {TestReport} testReport
 * @returns {string}
 */
const createTestsText = (testReport) => {
  const { passed, failed, skipped, time } = testReport;
  const total = passed + failed + skipped;
  return `${bold(total)} ${pluralize(total, 'test', 's')} were completed in ${bold(time + 'ms')}, with ${bold(passed)} passed, ${bold(failed)} failed and ${bold(skipped)} skipped.`;
};

/**
 * @typedef CloudflareDeployment
 * @property {string} id
 * @property {string} url
 * @property {string} environment
 * @property {string} projectName
 * @property {string} logsUrl
 */

/**
 * @typedef DiagnosticInfo
 * @property {TestReport} testReport
 * @property {CloudflareDeployment} deployment
 * @property {Record<string, unknown>} runInfo
 * @property {import("@actions/github/lib/context").Context} context
 */

/**
 * @param {DiagnosticInfo} info
 * @returns {string}
 */
const createDiagnostic = (info) => {
  const text = JSON.stringify(info, null, 2);
  const code = codeblock(text);
  return createTag("pre", code, true, { lang: 'json' });
}

/**
 * @param {object} context
 * @param {import("@actions/github/lib/context").Context} context.context
 * @param {import("@actions/core")} context.core
 * @param {boolean} context.debug
 * @returns {Promise<string>}
 */
const main = async ({ context, core, debug }) => {
  const commitSHA = process.env.COMMIT_SHA || context.sha;
  if (!isValidString(commitSHA)) {
    throw new Error(`Invalid commit SHA, received: ${commitSHA}`);
  }

  const projectName = process.env.CLOUDFLARE_PROJECT_NAME;
  if (!isValidString(projectName)) {
    throw new Error(`Invalid project name, received ${projectName}`);
  }

  const isInitialEdit = process.env.IS_INITIAL_EDIT === "true";
  const buildFailed = process.env.BUILD_CONCLUSION === "failed";

  const testReport = JSON.parse(process.env.TEST_REPORT || "{}");

  /** @type {CloudflareDeployment} */
  const deployment = JSON.parse(process.env.CLOUDFLARE || "{}");
  /** @type {Record<string, unknown>} */
  const runInfo = JSON.parse(process.env.RUN_INFO || "{}");

  const deploymentId = deployment.id;
  const deploymentUrl = deployment.url;
  const deploymentLogsUrl = `https://dash.cloudflare.com/?to=/:account/pages/view/${projectName}/${deploymentId}`;

  const imgUrl = "https://user-images.githubusercontent.com/23264/106598434-9e719e00-654f-11eb-9e59-6167043cfa01.png";
  const imgTag = createTag("img", "", false, {
    alt: "Cloudflare Pages",
    src: imgUrl,
    width: 16
  });
  const linkTag = createTag("a", imgTag, true, {
    href: "https://pages.dev/"
  });

  /** @type {import("@actions/core/lib/summary").SummaryTableRow[]} */
  const table = [
    [
      { data: bold("Latest commit:") },
      { data: codeblock(commitSHA.substring(0, 7)) },
    ],
    [
      { data: bold("Status:") },
    ],
  ];

  if (isValidString(deploymentUrl)) {
    table[1].push({
      data: `${spacing(emojis.build.success)} Deploy successful!`,
    });
    const deploymentLinkTag = createTag("a", deploymentUrl, true, {
      href: deploymentUrl,
    });
    table.push([
      { data: bold("Preview URL:") },
      { data: deploymentLinkTag },
    ]);
  } else if (isInitialEdit && !buildFailed) {
    table[1].push({
      data: `${spacing(emojis.build.progress)} Build in progress...`,
    });
  } else {
    table[1].push({
      data: `${spacing(emojis.build.failed)} Build failed.`,
    });
  }

  const summary = core.summary;

  if (validateTestReport(testReport) && !isInitialEdit) {
    summary
      .addHeading(`Test Results ${spacing(testReport.conclusion === "success" ? emojis.check : emojis.times)}`, 2)
      .addImage(
        createTestsBadge(testReport),
        `Tests: ${testReport.passed} passed${testReport.conclusion !== "success" ? `, ${testReport.failed} failed` : ''}`
      )
      .addRaw(createTestsText(testReport))
  }

  summary
    .addHeading(`Deploying LivingDoc with ${spacing(linkTag)} Cloudflare Pages`, 2)
    .addTable(table);

  if (isValidString(deploymentId)) {
    summary.addLink("View logs", deploymentLogsUrl);
  }

  summary
    .addSeparator()
    .addDetails(`Diagnostic Information: What the bot saw about this commit`, createDiagnostic({
      testReport,
      deployment: {
        ...deployment,
        projectName,
        logsUrl: deploymentLogsUrl,
      },
      runInfo,
      context,
    }));

  const html = summary.stringify();

  if (isInitialEdit || debug) {
    await summary.emptyBuffer().clear();
  } else {
    await summary.write();
  }

  return html;
};

module.exports = main;
