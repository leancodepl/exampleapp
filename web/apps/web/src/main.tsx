/* eslint-disable perfectionist/sort-imports */
/// <reference types="vite-plugin-svgr/client" />
import "@bart-krakowski/get-week-info-polyfill"
import { StrictMode } from "react"
import { Snackbar, Tooltip } from "@web/design-system"
import { QueryClientProvider } from "@tanstack/react-query"
import { createRouter, RouterProvider } from "@tanstack/react-router"
import * as ReactDOM from "react-dom/client"
import { KratosContextProvider } from "@leancodepl/kratos"
import { queryClient } from "./api"
import { kratosComponents } from "./auth/kratosComponents"
import { useHandleFlowError } from "./auth/useHandleFlowError"
import { routeTree } from "./routeTree.gen"
import "@pigment-css/react/styles.css"
import { ThemeProvider } from "./components/Theme"
import { LanguageProvider } from "./language/LanguageContext"

const router = createRouter({ routeTree, scrollRestoration: true })

declare module "@tanstack/react-router" {
  interface Register {
    router: typeof router
  }
}

const root = ReactDOM.createRoot(document.getElementById("root") as HTMLElement)

root.render(
  <StrictMode>
    <ThemeProvider>
      <LanguageProvider>
        <Tooltip.Provider>
          <QueryClientProvider client={queryClient}>
            <KratosContextProvider components={kratosComponents} useHandleFlowError={useHandleFlowError}>
              <Snackbar.Provider>
                <RouterProvider router={router} />
              </Snackbar.Provider>
            </KratosContextProvider>
          </QueryClientProvider>
        </Tooltip.Provider>
      </LanguageProvider>
    </ThemeProvider>
  </StrictMode>,
)
