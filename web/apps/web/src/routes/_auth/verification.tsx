import { createFileRoute, Navigate } from "@tanstack/react-router"
import * as v from "valibot"
import { flowIdParameterName, returnToParameterName } from "@leancodepl/kratos"
import { Verification } from "../../auth/flows/Verification"
import { useIsLoggedIn } from "../../auth/useIsLoggedIn"

const searchSchema = v.object({
  [returnToParameterName]: v.optional(v.string()),
  [flowIdParameterName]: v.optional(v.string()),
})

export const Route = createFileRoute("/_auth/verification")({
  validateSearch: searchSchema,
  component: VerificationPage,
})

function VerificationPage() {
  const searchParams = Route.useSearch()
  const isLoggedIn = useIsLoggedIn()

  if (isLoggedIn) {
    return <Navigate to="/" />
  }

  if (isLoggedIn === undefined) {
    return null
  }

  return <Verification searchParams={searchParams} />
}
