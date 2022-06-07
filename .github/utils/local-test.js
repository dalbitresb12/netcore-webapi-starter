const fs = require("fs");
const core = require("@actions/core");
const script = require("./cloudflare-summary.js");

if (!fs.existsSync("summary.txt")) {
  fs.writeFileSync("summary.txt", "", { encoding: "utf-8" });
}

const environment = {
  CI: true,
  GITHUB_STEP_SUMMARY: "summary.txt",
  COMMIT_SHA: "5ec689eeecdbeccd6de488fd3ab6d61561e33404",
  CLOUDFLARE_PROJECT_NAME: "netcore-webapi-starter-livingdoc",
  IS_INITIAL_EDIT: ["false", "true"][0],
  BUILD_CONCLUSION: ["success", "failed"][1],
  TEST_REPORT: JSON.stringify({
    conclusion: ["success", "failure"][0],
    passed: "4",
    failed: "0",
    skipped: "0",
    time: "3176",
  }),
  CLOUDFLARE: JSON.stringify({
    id: "4582ada4-b99f-4599-bb12-0bf9a2e6291d",
    url: "https://4582ada4.netcore-webapi-starter-livingdoc.pages.dev",
    environment: "production",
    projectName: "netcore-webapi-starter-livingdoc",
    logsUrl: "https://dash.cloudflare.com/?to=/:account/pages/view/netcore-webapi-starter-livingdoc/4582ada4-b99f-4599-bb12-0bf9a2e6291d",
  }),
  RUN_INFO: JSON.stringify({
    sourceHeadRepo: "dalbitresb12/netcore-webapi-starter",
    sourceHeadBranch: "main",
    sourceHeadSha: "5ec689eeecdbeccd6de488fd3ab6d61561e33404",
    sourceEvent: "push",
    pullRequestNumber: "",
    pullRequestLabels: JSON.stringify([]),
    mergeCommitSha: "",
    targetCommitSha: "5ec689eeecdbeccd6de488fd3ab6d61561e33404",
    targetBranch: "main",
  }),
};

Object.entries(environment).forEach(([key, value]) => {
  process.env[key] = value;
});

script({ context: {}, core, debug: true })
  .then(console.log)
  .catch(console.error);
