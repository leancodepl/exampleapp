import { useCallback, useEffect, useMemo } from "react"
import { ContinueWith, SettingsFlow, UiNode } from "@ory/client"
import { useNavigate } from "@tanstack/react-router"
import { SkeletonLoader } from "@web/design-system"
import { KratosContextProvider, UserSettingsCard } from "@leancodepl/kratos"
import { useBoundRunInTask } from "@leancodepl/utils"
import { kratosClient } from "../.."
import { AuthContainer } from "../../components/AuthContainer"
import { useSettingsFlow } from "../../hooks/useSettingsFlow"
import { sessionManager } from "../../sessionManager"
import { AuthContextProvider } from "../../utils/authContext"
import { passwordComponents } from "../../utils/passwordComponents"

export function PasswordSettingsFlow() {
  const nav = useNavigate({ from: "/" })

  const onContinueWith = useCallback(
    (continueWith: ContinueWith[]) => {
      for (const continuation of continueWith) {
        switch (continuation.action) {
          case "redirect_browser_to":
            window.location.href = continuation.redirect_browser_to
            break
          case "show_recovery_ui":
            nav({ to: "/recovery", search: { flow: continuation.flow.id } })
            break
          case "show_settings_ui":
            break
          case "show_verification_ui":
            break
        }
      }
    },
    [nav],
  )

  const { flow, submit } = useSettingsFlow({
    kratosClient,
    onContinueWith,
    returnTo: "/profile/password",
  })

  const passwordFlow = useMemo(() => getPasswordFlow(flow), [flow])

  const [isLoadingPassword, submitPassword] = useBoundRunInTask(submit)

  useEffect(() => {
    if (!flow) return

    if (flow.state === "success") {
      sessionManager.checkIfLoggedIn()
      nav({ to: "/" })
    }
  }, [flow, nav])

  return (
    <AuthContainer>
      {passwordFlow ? (
        <KratosContextProvider components={passwordComponents}>
          <AuthContextProvider isLoading={isLoadingPassword}>
            <UserSettingsCard flow={passwordFlow} flowType="password" onSubmit={submitPassword} />
          </AuthContextProvider>
        </KratosContextProvider>
      ) : (
        <SkeletonLoader height={230} />
      )}
    </AuthContainer>
  )
}

function filterFlowNodes(flow: SettingsFlow | undefined, filter: (node: UiNode) => boolean) {
  if (!flow) return undefined

  return {
    ...flow,
    ui: {
      ...flow.ui,
      nodes: flow.ui.nodes.filter(filter),
    },
  }
}

function getPasswordFlow(flow?: SettingsFlow) {
  return filterFlowNodes(flow, node => node.group === "password" || node.group === "default")
}
