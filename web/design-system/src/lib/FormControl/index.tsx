import { ReactNode, useState } from "react"
import { useSetUnset } from "@leancodepl/utils"
import { dataError, dataLabelInHeader } from "./attributes"
import { FormControlProvider } from "./FormControlProvider"
import { FormControlRoot } from "./FormControlRoot"

export type FormControlBaseProps = {
  hasValue?: boolean
  hasError?: boolean
  children?: ReactNode
  className?: string
}

export function FormControlBase({ hasValue, hasError, className, children }: FormControlBaseProps) {
  const [hasFocus, setHasFocus] = useState(false)
  const [handleFocus, handleBlur] = useSetUnset(setHasFocus)

  const labelInHeader = hasFocus || hasValue

  return (
    <FormControlRoot
      className={className}
      onBlur={handleBlur}
      onFocus={handleFocus}
      {...dataLabelInHeader(labelInHeader ? "" : undefined)}
      {...dataError(hasError ? "" : undefined)}>
      <FormControlProvider>{children}</FormControlProvider>
    </FormControlRoot>
  )
}

export * from "./FormControlContainer"
export * from "./FormControlLabel"
export * from "./FormControlError"
export * from "./FormControlHint"
export * from "./FormControlValue"
