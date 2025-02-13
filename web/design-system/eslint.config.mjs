import nx from "@nx/eslint-plugin"
import baseConfig from "../eslint.config.mjs"

// eslint-disable-next-line import/no-anonymous-default-export
export default [
  ...baseConfig,
  ...nx.configs["flat/react"],
  {
    files: ["**/*.ts", "**/*.tsx", "**/*.js", "**/*.jsx"],
    // Override or add rules here
    rules: {},
  },
]
