import { createFileRoute, Outlet, redirect, useNavigate } from "@tanstack/react-router"
import { Sidebar } from "@web/design-system"
import { firstValueFrom } from "rxjs"
import { returnToParameterName } from "@leancodepl/kratos"
import { sessionManager } from "../auth/sessionManager"
import { useIsLoggedIn } from "../auth/useIsLoggedIn"
import { AppSidebar } from "../components/AppSidebar"
import { AuthorizedLayout } from "../components/AuthorizedLayout"

export const estateIdSearchParam = "estateId"

export const Route = createFileRoute("/_authorized")({
  component: AuthorizedPage,
  beforeLoad: async ({ location }) => {
    const isLoggedIn = await firstValueFrom(sessionManager.isLoggedIn)

    if (!isLoggedIn) {
      throw redirect({
        to: "/login",
        search: { [returnToParameterName]: location.href },
      })
    }
  },
})

function AuthorizedPage() {
  const isLoggedIn = useIsLoggedIn()
  const nav = useNavigate()

  if (isLoggedIn === false) {
    nav({ to: "/login", search: { [returnToParameterName]: window.location.href } })
    return null
  }

  return (
    <Sidebar.Provider>
      <AppSidebar />

      <AuthorizedLayout>
        <Outlet />
      </AuthorizedLayout>
    </Sidebar.Provider>
  )
}
