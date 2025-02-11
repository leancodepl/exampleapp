import { useCallback, useEffect } from "react"
import { FormattedMessage } from "react-intl"
import { createFileRoute, Navigate, useNavigate } from "@tanstack/react-router"
import { Snackbar } from "@web/design-system"
import { useLogoutFlow } from "@leancodepl/kratos"
import { kratosClient } from "../../../auth"
import { sessionManager } from "../../../auth/sessionManager"
import { useIsLoggedIn } from "../../../auth/useIsLoggedIn"

export const Route = createFileRoute("/_auth/login/expired")({
  component: RouteComponent,
})

function RouteComponent() {
  const nav = useNavigate()
  const isLoggedIn = useIsLoggedIn()

  const { logout } = useLogoutFlow({
    kratosClient,
    onLoggedOut: useCallback(() => sessionManager.checkIfLoggedIn(), []),
  })

  const { info, hideAllSnackbars } = Snackbar.use()

  useEffect(() => {
    hideAllSnackbars()
    info({
      title: (
        <FormattedMessage
          defaultMessage="Twoja sesja straciła ważność. Odzyskaj dostęp do konta"
          id="auth.login.message.sessionExpired"
        />
      ),
    })
    ;(async () => {
      try {
        await logout()
      } finally {
        /* empty */
      }
    })()
  }, [logout, hideAllSnackbars, info, nav])

  if (isLoggedIn) {
    return <Navigate to="/recovery" />
  }

  return null
}
