import { ReactNode } from "@tanstack/react-router"
import { useFormControl } from "../FormControlProvider"
import { FormControlValueLabel, FormControlValueWrapper } from "./styles"

type FormControlValueProps = {
  children?: ReactNode
  className?: string
}

export function FormControlValue({ className, children }: FormControlValueProps) {
  const { label } = useFormControl()

  return (
    <FormControlValueWrapper className={className}>
      {label && <FormControlValueLabel>{label}</FormControlValueLabel>}
      {children}
    </FormControlValueWrapper>
  )
}
