import { ChangeEventHandler, ComponentPropsWithoutRef, forwardRef, ReactNode, useCallback } from "react"
import { FormControlBase, FormControlError, FormControlHint } from "../FormControl"
import { Text } from "../Text"
import { CheckboxContainer, CheckIndicator, CheckInput } from "./styles"

type CheckboxProps = {
  error?: boolean | ReactNode
  hint?: ReactNode

  children?: ReactNode
  onCheckedChange?: (checked: boolean) => void
} & Omit<ComponentPropsWithoutRef<"input">, "type">

export const Checkbox = forwardRef<HTMLInputElement, CheckboxProps>(
  ({ children, onCheckedChange, onChange, error, hint, ...props }, ref) => {
    const handleChange = useCallback<ChangeEventHandler<HTMLInputElement>>(
      e => {
        onCheckedChange?.(e.target.checked)
        onChange?.(e)
      },
      [onChange, onCheckedChange],
    )

    return (
      <FormControlBase hasError={!!error}>
        <CheckboxContainer>
          <CheckInput ref={ref} {...props} type="checkbox" onChange={handleChange} />
          <CheckIndicator fill="none" height="18" viewBox="0 0 18 18" width="18" xmlns="http://www.w3.org/2000/svg">
            <path d="M4.5 9 L7.5 12 L13.5 6" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" />
          </CheckIndicator>
          <Text body color="default.primary">
            {children}
          </Text>
        </CheckboxContainer>

        {hint && <FormControlHint caption>{hint}</FormControlHint>}

        {error && error !== true && <FormControlError caption>{error}</FormControlError>}
      </FormControlBase>
    )
  },
)
