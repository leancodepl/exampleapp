import { createFileRoute, Outlet } from "@tanstack/react-router"
import { UnauthorizedLayout } from "../components/UnauthorizedLayout"

export const Route = createFileRoute("/_auth")({
  component: AuthPage,
})

function AuthPage() {
  return (
    <UnauthorizedLayout>
      <Outlet />
    </UnauthorizedLayout>
  )
}
