import { ChangeEventHandler, ComponentPropsWithoutRef, useCallback, useState } from "react"
import { ReactNode } from "@tanstack/react-router"
import {
  FormControlBase,
  FormControlContainer,
  FormControlError,
  FormControlHint,
  FormControlLabel,
  FormControlValue,
} from "../FormControl"
import { dataSize } from "../FormControl/attributes"
import { InputRoot } from "./styles"

type InputProps = {
  error?: boolean | ReactNode
  hint?: ReactNode

  value?: string
  onValueChange?: (value: string) => void

  label?: ReactNode
  leading?: ReactNode
  trailing?: ReactNode

  large?: boolean
} & Omit<ComponentPropsWithoutRef<"input">, "hint">

export function Input({
  disabled,
  error,
  hint,

  value,

  onChange,
  onValueChange,

  leading,
  label,
  trailing,

  large,

  ...props
}: InputProps) {
  const [internalValue, setInternalValue] = useState(value)

  const handleChange = useCallback<ChangeEventHandler<HTMLInputElement>>(
    e => {
      setInternalValue(e.target.value)
      onChange?.(e)
      onValueChange?.(e.target.value)
    },
    [onChange, onValueChange],
  )

  const effectiveValue = value ?? internalValue

  return (
    <FormControlBase hasError={!!error} hasValue={!!effectiveValue}>
      <FormControlLabel>{label}</FormControlLabel>

      <FormControlContainer {...dataSize(large ? "large" : "normal")}>
        {leading}
        <FormControlValue>
          <InputRoot disabled={disabled} value={effectiveValue} onChange={handleChange} {...props} />
        </FormControlValue>
        {trailing}
      </FormControlContainer>

      {hint && <FormControlHint caption>{hint}</FormControlHint>}
      {error && error !== true && <FormControlError caption>{error}</FormControlError>}
    </FormControlBase>
  )
}
