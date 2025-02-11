import playwright from "eslint-plugin-playwright"
import baseConfig from "../../eslint.config.mjs"

// eslint-disable-next-line import/no-anonymous-default-export
export default [
  playwright.configs["flat/recommended"],
  ...baseConfig,
  {
    files: ["**/*.ts", "**/*.js"],
    // Override or add rules here
    rules: {},
  },
]
