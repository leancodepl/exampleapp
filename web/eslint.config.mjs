/* eslint-disable import/no-extraneous-dependencies */
import nx from "@nx/eslint-plugin"
import react from "eslint-plugin-react"
import globals from "globals"
import leancode from "@leancodepl/eslint-config"

const baseReact = [
  {
    plugins: {
      react,
    },
    languageOptions: {
      parserOptions: {
        ecmaFeatures: {
          jsx: true,
        },
      },
      globals: {
        ...globals.browser,
      },
    },
    rules: {
      "@typescript-eslint/no-non-null-assertion": "off",

      "react/jsx-no-useless-fragment": ["error", { allowExpressions: true }],
      "react/jsx-boolean-value": "error",
      "react/jsx-curly-brace-presence": "warn",
      "react/jsx-fragments": "warn",
      "react/jsx-sort-props": [
        "warn",
        {
          callbacksLast: true,
          shorthandFirst: true,
          shorthandLast: false,
          ignoreCase: true,
          noSortAlphabetically: false,
          reservedFirst: true,
        },
      ],
      "react/style-prop-object": [
        "warn",
        {
          allow: ["FormattedNumber"],
        },
      ],
      "react/self-closing-comp": "error",
    },
  },
]

const { plugins: _plugins1, ..._baseReactWithoutPlugins } = leancode.baseReact[0]
const { plugins: _plugins2, ..._baseA11yWithoutPlugins } = leancode.a11y[0]

// eslint-disable-next-line import/no-anonymous-default-export
export default [
  ...nx.configs["flat/base"],
  ...nx.configs["flat/typescript"],
  ...nx.configs["flat/javascript"],
  {
    ignores: ["**/dist"],
  },
  {
    files: ["**/*.ts", "**/*.tsx", "**/*.js", "**/*.jsx"],
    rules: {
      "@nx/enforce-module-boundaries": [
        "error",
        {
          enforceBuildableLibDependency: true,
          allow: ["^.*/eslint(\\.base)?\\.config\\.[cm]?js$"],
          depConstraints: [
            {
              sourceTag: "*",
              onlyDependOnLibsWithTags: ["*"],
            },
          ],
        },
      ],
    },
  },
  {
    files: ["**/*.ts", "**/*.tsx", "**/*.cts", "**/*.mts", "**/*.js", "**/*.jsx", "**/*.cjs", "**/*.mjs"],
    // Override or add rules here
    rules: {},
  },
  ...baseReact,
  ...leancode.base,
  ...leancode.imports,
]
