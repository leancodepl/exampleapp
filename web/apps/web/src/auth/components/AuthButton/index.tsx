import { useMemo } from "react"
import { Button, ButtonType } from "@web/design-system"
import { ButtonComponentProps, InfoNodeLabel } from "@leancodepl/kratos"
import { useAuthContext } from "../../utils/authContext"

export function AuthButton({ header, fullWidth: _fullWidth, node, type, ...props }: ButtonComponentProps) {
  const buttonType: ButtonType = ["oidc", "passkey"].includes(node.group)
    ? "secondary"
    : node.meta.label?.id === InfoNodeLabel.InfoNodeLabelResendOTP
      ? "tertiary"
      : "primary"

  const { onClick, onLoad } = useMemo(() => {
    if (node.attributes.node_type !== "input") return {}

    const onclick = node.attributes.onclick
    const onload = node.attributes.onload

    return {
      onClick: onclick
        ? () => {
            // eslint-disable-next-line no-eval
            eval(onclick)
          }
        : undefined,

      // eslint-disable-next-line no-eval
      onLoad: onload ? () => eval(onload) : undefined,
    }
  }, [node])

  const { isLoading } = useAuthContext()

  return (
    <Button
      {...props}
      block
      htmlType={type}
      isLoading={isLoading}
      size="large"
      type={buttonType}
      onClick={onClick}
      onLoad={onLoad}>
      {header}
    </Button>
  )
}
