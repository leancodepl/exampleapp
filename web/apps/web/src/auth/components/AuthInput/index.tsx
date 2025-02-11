import { useMemo } from "react"
import { Input } from "@web/design-system"
import { IconLock01, IconMail02, IconUser01 } from "@web/design-system/icons"
import { InfoNodeLabel, InputComponentProps } from "@leancodepl/kratos"

export function AuthInput({
  header,
  helperMessage,
  size: _size,
  isError,
  node,
  prefix: _prefix,
  value: _value,
  ...props
}: InputComponentProps) {
  const icon = useMemo(() => {
    switch (node.meta.label?.id) {
      case InfoNodeLabel.InfoNodeLabelInputPassword:
        return <IconLock01 />
      case InfoNodeLabel.InfoNodeLabelEmail:
        return <IconMail02 />
      case InfoNodeLabel.InfoNodeLabelGenerated:
        if (node.attributes.node_type !== "input") return null

        switch (node.attributes.name) {
          case "traits.given_name":
            return <IconUser01 />
          case "traits.family_name":
            return <IconUser01 />
          case "traits.email":
          case "identifier":
            return <IconMail02 />
        }
    }
  }, [node])

  if (props.type === "hidden") return <input {...props} />

  return (
    <Input.Input
      {...props}
      large
      error={isError && helperMessage}
      hint={!isError && helperMessage}
      label={header}
      leading={icon}
    />
  )
}
