import { ComponentPropsWithoutRef } from "react"
import * as SelectPrimitive from "@radix-ui/react-select"
import { ReactNode } from "@tanstack/react-router"
import { FormControlBase, FormControlError, FormControlHint, FormControlLabel } from "../FormControl"
import { IconChevronDown } from "../icons"
import { Text } from "../Text"
import { SelectFormControlContainer, SelectPrimitiveIcon } from "./styles"

type SelectRootProps = {
  placeholder?: ReactNode
  className?: string

  error?: boolean | ReactNode
  hint?: ReactNode
  label?: ReactNode
} & ComponentPropsWithoutRef<typeof SelectPrimitive.Root>

export function Root({ children, placeholder, className, error, hint, label, ...props }: SelectRootProps) {
  return (
    <FormControlBase hasValue hasError={!!error}>
      <SelectPrimitive.Root {...props}>
        {label && <FormControlLabel>{label}</FormControlLabel>}

        <SelectPrimitive.Trigger asChild>
          <SelectFormControlContainer as="button" className={className}>
            <Text contents color="default.primary">
              <SelectPrimitive.Value placeholder={placeholder} />
            </Text>
            <SelectPrimitiveIcon>
              <IconChevronDown />
            </SelectPrimitiveIcon>
          </SelectFormControlContainer>
        </SelectPrimitive.Trigger>

        {children}

        {hint && <FormControlHint caption>{hint}</FormControlHint>}
        {error && error !== true && <FormControlError caption>{error}</FormControlError>}
      </SelectPrimitive.Root>
    </FormControlBase>
  )
}

export { SelectContent as Content } from "./SelectContent"
export { SelectItem as Item } from "./SelectItem"
