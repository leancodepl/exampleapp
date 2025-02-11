import { createFileRoute } from "@tanstack/react-router"
import * as v from "valibot"
import { returnToParameterName } from "@leancodepl/kratos"
import { Login } from "../../../auth/flows/Login"
import { useIsLoggedIn } from "../../../auth/useIsLoggedIn"

const searchSchema = v.object({
  [returnToParameterName]: v.optional(v.string()),
})

export const Route = createFileRoute("/_auth/login/")({
  validateSearch: searchSchema,
  component: LoginPage,
})

function LoginPage() {
  const searchParams = Route.useSearch()
  const isLoggedIn = useIsLoggedIn()

  if (isLoggedIn === undefined) {
    return null
  }

  return <Login isLoggedIn={isLoggedIn} searchParams={searchParams} />
}
