import { Checkbox } from "@web/design-system"
import { CheckboxComponentProps } from "@leancodepl/kratos"

export function AuthCheckbox({ label, helperMessage, isError, node: _node, ...props }: CheckboxComponentProps) {
  if (props.type === "hidden") return <input {...props} />

  return (
    <Checkbox {...props} error={isError && helperMessage} hint={!isError && helperMessage}>
      {label}
    </Checkbox>
  )
}
