import { useCallback, useEffect, useState } from "react"
import { ContinueWith, FrontendApi, SettingsFlow, UpdateSettingsFlowBody } from "@ory/client"
import { AxiosError, AxiosRequestConfig } from "axios"
import { useKratosContext } from "@leancodepl/kratos"
import { handleCancelError } from "../utils/handleCancelError"

export function useSettingsFlow({
  kratosClient,
  params,
  onContinueWith,
  settingsFlowId,
  returnTo,
}: {
  kratosClient: FrontendApi
  params?: AxiosRequestConfig["params"]
  onContinueWith?: (continueWith: ContinueWith[]) => void
  returnTo?: string
  settingsFlowId?: string
}) {
  const { useHandleFlowError } = useKratosContext()

  const [flowIdInternal, setFlowId] = useState<string>()
  const [flow, setFlow] = useState<SettingsFlow>()

  const flowId = settingsFlowId ?? flowIdInternal

  const handleFlowError = useHandleFlowError({
    resetFlow: useCallback(() => {
      setFlowId(undefined)
      setFlow(undefined)
    }, []),
  })

  useEffect(() => {
    if (flow) return

    const controller = new AbortController()

    if (flowId) {
      kratosClient
        .getSettingsFlow({ id: flowId }, { signal: controller.signal })
        .then(({ data }) => setFlow(data))
        .catch(handleCancelError)
        .catch(handleFlowError)
    } else {
      kratosClient
        .createBrowserSettingsFlow(
          {
            returnTo,
          },
          {
            params,
            signal: controller.signal,
          },
        )
        .then(({ data }) => setFlow(data))
        .catch(handleCancelError)
        .catch(handleFlowError)
    }

    return () => {
      controller.abort()
    }
  }, [flow, flowId, handleFlowError, kratosClient, params, returnTo])

  const submit = useCallback(
    ({ body }: { body: UpdateSettingsFlowBody }) => {
      if (!flow) return

      setFlowId(flow.id)

      return kratosClient
        .updateSettingsFlow({ flow: flow.id, updateSettingsFlowBody: body })
        .then(({ data }) => {
          if (flow.return_to) {
            window.location.href = flow.return_to
            return
          }

          setFlow(data)

          if (data.continue_with) {
            onContinueWith?.(data.continue_with)
          }
        })
        .catch(handleFlowError)
        .catch((err: AxiosError<SettingsFlow>) => {
          if (err.response?.status === 400) {
            setFlow(err.response?.data)
            return
          }

          return Promise.reject(err)
        })
    },
    [flow, kratosClient, handleFlowError, onContinueWith],
  )

  return { flow, submit }
}
