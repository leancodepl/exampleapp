import { useCallback } from "react"
import { createFileRoute, Navigate, useNavigate } from "@tanstack/react-router"
import * as v from "valibot"
import { flowIdParameterName, returnToParameterName } from "@leancodepl/kratos"
import { Register } from "../../auth/flows/Register"
import { useIsLoggedIn } from "../../auth/useIsLoggedIn"

const searchSchema = v.object({
  [returnToParameterName]: v.optional(v.string()),
  [flowIdParameterName]: v.optional(v.string()),
})

export const Route = createFileRoute("/_auth/registration")({
  validateSearch: searchSchema,
  component: RegisterPage,
})

function RegisterPage() {
  const searchParams = Route.useSearch()
  const isLoggedIn = useIsLoggedIn()

  const nav = useNavigate()

  const onShowVerificationUi = useCallback(
    (verificationFlowId: string) => {
      nav({ to: "/verification", search: { flow: verificationFlowId } })
    },
    [nav],
  )

  if (isLoggedIn) {
    return <Navigate to="/" />
  }

  if (isLoggedIn === undefined) {
    return null
  }

  return <Register searchParams={searchParams} onShowVerificationUi={onShowVerificationUi} />
}
