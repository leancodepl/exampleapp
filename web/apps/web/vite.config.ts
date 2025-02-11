/// <reference types='vitest' />
import { nxCopyAssetsPlugin } from "@nx/vite/plugins/nx-copy-assets.plugin"
import { nxViteTsPaths } from "@nx/vite/plugins/nx-tsconfig-paths.plugin"
import { extendTheme, pigment } from "@pigment-css/vite-plugin"
import { TanStackRouterVite } from "@tanstack/router-plugin/vite"
import react from "@vitejs/plugin-react"
import { defineConfig } from "vite"
import svgr from "vite-plugin-svgr"
import { defaultDarkColors, defaultLightColors, defaultTheme } from "../../design-system/src/theme"

export default defineConfig({
  root: __dirname,
  cacheDir: "../../node_modules/.vite/apps/web",
  server: {
    port: 4200,
    host: "0.0.0.0",
    allowedHosts: ["local.lncd.pl"],
  },
  preview: {
    port: 4300,
    host: "localhost",
  },
  plugins: [
    react(),
    nxViteTsPaths(),
    nxCopyAssetsPlugin(["*.md"]),
    pigment({
      displayName: true,
      theme: extendTheme({
        colorSchemes: {
          light: defaultLightColors,
          dark: defaultDarkColors,
        },
        ...defaultTheme,
        getSelector: colorScheme => (colorScheme === "dark" ? `[data-theme="dark"]` : ":root"),
      }),
    }),
    TanStackRouterVite(),
    svgr(),
    svgr({
      include: "**/*.svg?icon",
      svgrOptions: {
        template: (variables, { tpl }) => tpl`
          ${variables.imports}
          import { IconBase } from "./_base"

          ${variables.interfaces}

          const ${variables.componentName} = (${variables.props}) => (
            ${(() => {
              variables.jsx.openingElement.name = { type: "JSXIdentifier", name: "IconBase" }

              if (variables.jsx.closingElement) {
                variables.jsx.closingElement.name = variables.jsx.openingElement.name
              }

              variables.jsx.openingElement.attributes.splice(0, 2, {
                type: "JSXSpreadAttribute",
                argument: { type: "Identifier", name: "props" },
              })

              return variables.jsx
            })()}
          )

          ${variables.exports}
        `,
      },
    }),
  ],
  // Uncomment this if you are using workers.
  // worker: {
  //  plugins: [ nxViteTsPaths() ],
  // },
  build: {
    outDir: "../../dist/apps/web",
    emptyOutDir: true,
    reportCompressedSize: true,
    commonjsOptions: {
      transformMixedEsModules: true,
    },
  },
  test: {
    watch: false,
    globals: true,
    environment: "jsdom",
    include: ["src/**/*.{test,spec}.{js,mjs,cjs,ts,mts,cts,jsx,tsx}"],
    reporters: ["default"],
    coverage: {
      reportsDirectory: "../../coverage/apps/web",
      provider: "v8",
    },
    passWithNoTests: true,
  },
})
