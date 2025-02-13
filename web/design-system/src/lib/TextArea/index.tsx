import { ChangeEventHandler, useCallback, useState } from "react"
import { TextareaAutosizeProps } from "react-textarea-autosize"
import { ReactNode } from "@tanstack/react-router"
import { FormControlBase, FormControlError, FormControlHint, FormControlLabel } from "../FormControl"
import { dataSize } from "../FormControl/attributes"
import { TextAreaFormControlContainer, TextAreaFormControlValue, TextAreaRoot } from "./styles"

type TextAreaProps = {
  error?: boolean | ReactNode
  hint?: ReactNode

  value?: string
  onValueChange?: (value: string) => void

  label?: ReactNode

  large?: boolean
} & Omit<TextareaAutosizeProps, "error" | "hint">

export function TextArea({
  disabled,
  error,
  hint,

  value,

  onValueChange,
  onChange,

  label,

  large,

  ...props
}: TextAreaProps) {
  const [internalValue, setInternalValue] = useState(value)

  const handleChange = useCallback<ChangeEventHandler<HTMLTextAreaElement>>(
    e => {
      setInternalValue(e.target.value)
      onChange?.(e)
      onValueChange?.(e.target.value)
    },
    [onChange, onValueChange],
  )

  return (
    <FormControlBase hasError={!!error} hasValue={!!internalValue}>
      <FormControlLabel>{label}</FormControlLabel>

      <TextAreaFormControlContainer {...dataSize(large ? "large" : "normal")}>
        <TextAreaFormControlValue>
          <TextAreaRoot disabled={disabled} value={value} onChange={handleChange} {...props} />
        </TextAreaFormControlValue>
      </TextAreaFormControlContainer>

      {hint && <FormControlHint caption>{hint}</FormControlHint>}
      {error && error !== true && <FormControlError caption>{error}</FormControlError>}
    </FormControlBase>
  )
}
