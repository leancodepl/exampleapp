import { useCallback } from "react"
import { useNavigate } from "@tanstack/react-router"
import { ErrorId, returnToParameterName, UseHandleFlowError } from "@leancodepl/kratos"

export const useHandleFlowError: UseHandleFlowError = ({
  resetFlow,
  onSessionAlreadyAvailable,
}: {
  resetFlow: (newFlowId?: string) => void
  onSessionAlreadyAvailable?: () => void
}) => {
  const nav = useNavigate()

  return useCallback(
    async err => {
      switch (err.response?.data?.error?.id) {
        case ErrorId.ErrNoActiveSession:
          nav({ to: "/login", search: { [returnToParameterName]: window.location.href } })
          return
        case ErrorId.ErrIDHigherAALRequired:
          // 2FA is enabled and enforced, but user did not perform 2fa yet!
          window.location.href = err.response.data.redirect_browser_to
          return
        case ErrorId.ErrIDAlreadyLoggedIn:
          onSessionAlreadyAvailable?.()
          return
        case ErrorId.ErrIDNeedsPrivilegedSession:
          // We need to re-authenticate to perform this action
          window.location.href = err.response.data.redirect_browser_to
          return
        case ErrorId.ErrIDRedirectURLNotAllowed:
          // TODO: display message
          // message.error(<FormattedMessage defaultMessage="The return_to address is not allowed." />)
          resetFlow()
          return
        case ErrorId.ErrIDSelfServiceFlowExpired:
          // The flow expired, let's request a new one.
          // TODO: display message
          // message.error(
          //     <FormattedMessage defaultMessage="Your interaction expired, please fill out the form again." />,
          // )
          resetFlow()
          return
        case ErrorId.ErrIDSelfServiceFlowReplaced:
          resetFlow(err.response.data.use_flow_id)
          return
        case ErrorId.ErrIDCSRF:
          // TODO: display message
          // message.error(
          //     <FormattedMessage defaultMessage="A security violation was detected, please fill out the form again." />,
          // )
          resetFlow()
          return
        case ErrorId.ErrIDInitiatedBySomeoneElse:
          // The requested item was intended for someone else. Let's request a new flow...
          resetFlow()
          return
        case ErrorId.ErrIDSelfServiceBrowserLocationChangeRequiredError:
          // Ory Kratos asked us to point the user to this URL.
          window.location.href = err.response.data.redirect_browser_to
          return
      }

      switch (err.response?.status) {
        case 401:
        case 410:
          resetFlow()
          return
      }

      return Promise.reject(err)
    },
    [nav, onSessionAlreadyAvailable, resetFlow],
  )
}
