import React, { Suspense } from "react"
import { ReactQueryDevtools } from "@tanstack/react-query-devtools"
import { createRootRoute, Outlet } from "@tanstack/react-router"
import { NotFound } from "../components/NotFound"

export const Route = createRootRoute({
  component: () => (
    <>
      <Outlet />
      <Suspense>
        <TanStackRouterDevtools />
      </Suspense>
      <ReactQueryDevtools initialIsOpen={false} />
    </>
  ),
  notFoundComponent: NotFound,
})

const TanStackRouterDevtools = import.meta.env.PROD
  ? () => null
  : React.lazy(() => import("@tanstack/router-devtools").then(res => ({ default: res.TanStackRouterDevtools })))
