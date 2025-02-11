import { createFileRoute, Navigate } from "@tanstack/react-router"
import * as v from "valibot"
import { flowIdParameterName, returnToParameterName } from "@leancodepl/kratos"
import { Recovery } from "../../auth/flows/Recovery"
import { useIsLoggedIn } from "../../auth/useIsLoggedIn"

const searchSchema = v.object({
  [returnToParameterName]: v.optional(v.string()),
  [flowIdParameterName]: v.optional(v.string()),
})

export const Route = createFileRoute("/_auth/recovery")({
  validateSearch: searchSchema,
  component: RecoveryPage,
})

function RecoveryPage() {
  const searchParams = Route.useSearch()
  const isLoggedIn = useIsLoggedIn()

  if (isLoggedIn) {
    return <Navigate to="/" />
  }

  if (isLoggedIn === undefined) {
    return null
  }

  return <Recovery searchParams={searchParams} />
}
