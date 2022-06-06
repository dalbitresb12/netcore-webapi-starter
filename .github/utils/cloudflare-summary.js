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
  const encoded = Object.keys(attributes).reduce((acc, key) => {
    acc += ` ${key}="${attributes[key]}"`;
    return acc;
  }, "");
  return `<${tag}${encoded}>${content}${endTag ? `</${tag}>` : ''}`;
};

/**
 * @param {string} str
 * @returns {string}
 */
const createCode = (str) => createTag("code", str);

/**
 * @param {string} str
 * @returns {string}
 */
const createStrong = (str) => createTag("strong", str);

const emojis = {
  build: {
    success: ':white_check_mark:',
    failed: ':no_entry_sign:',
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
  const { conclusion, passed } = testReport;
  if (conclusion === "success") {
    return `https://img.shields.io/badge/tests-${passed}%20passed-brightgreen`;
  }
  return `https://img.shields.io/badge/tests-${passed}%20passed-red;`;
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
  return `${total} ${pluralize(total, 'test', 's')} were completed in ${time}ms, with ${passed} passed, ${failed} failed and ${skipped} skipped.`;
};

/**
 * @param {object} context
 * @param {import("@actions/core")} context.core
 * @returns {string}
 */
const main = async ({ core }) => {
  const commitSHA = process.env.COMMIT_SHA;
  if (!isValidString(commitSHA)) {
    throw new Error(`Invalid commit SHA, received: ${commitSHA}`);
  }

  const projectName = process.env.CLOUDFLARE_PROJECT_NAME;
  if (!isValidString(projectName)) {
    throw new Error(`Invalid project name, received ${projectName}`);
  }

  const testReport = JSON.parse(process.env.TEST_REPORT || "{}");
  if (!validateTestReport(testReport)) {
    throw new Error(`Invalid test report, received ${testReport}`);
  }

  const deploymentId = process.env.DEPLOYMENT_ID;
  const deploymentUrl = process.env.DEPLOYMENT_URL;
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
      { data: createStrong("Latest commit:") },
      { data: createCode(commitSHA.substring(0, 7)) },
    ],
    [
      { data: createStrong("Status:") },
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
      { data: createStrong("Preview URL:") },
      { data: deploymentLinkTag },
    ]);
  } else {
    table[1].push({
      data: `${spacing(emojis.build.failed)} Build failed.`,
    });
  }

  const summary = core.summary
    .addHeading(`Test Results ${spacing(testReport.conclusion === "success" ? emojis.check : emojis.times)}`, 2)
    .addImage(createTestsBadge(testReport), `Tests: ${testReport.passed} passed`)
    .addRaw(createTestsText(testReport))
    .addHeading(`Deploying LivingDoc with ${spacing(linkTag)} Cloudflare Pages`, 2)
    .addTable(table);

  if (isValidString(deploymentId)) {
    summary.addLink("View logs", deploymentLogsUrl);
  }

  const html = summary.stringify();
  await summary.write();

  return html;
};

module.exports = main;
