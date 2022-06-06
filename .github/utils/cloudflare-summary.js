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
const createInlineCode = (str) => createTag("code", str);

/**
 * @param {string} str
 * @returns {string}
 */
const createStrong = (str) => createTag("strong", str);

/**
 * @param {object} context
 * @param {import("@actions/core")} context.core
 * @returns {Record<string, unknown>}
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
      { data: createInlineCode(commitSHA.substring(0, 7)) },
    ],
    [
      { data: createStrong("Status:") },
    ],
  ];

  if (isValidString(deploymentUrl)) {
    table[1].push({
      data: "&nbsp;:white_check_mark:&nbsp; Deploy successful!",
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
      data: "&nbsp;:no_entry_sign:&nbsp; Build failed.",
    });
  }

  const summary = core.summary
    .addHeading(`Deploying with &nbsp;${linkTag}&nbsp;Cloudflare Pages`, 2)
    .addTable(table);

  if (isValidString(deploymentId)) {
    summary.addLink("View logs", deploymentLogsUrl);
  }

  const html = summary.stringify();
  await summary.write();

  return { html };
};

module.exports = main;
