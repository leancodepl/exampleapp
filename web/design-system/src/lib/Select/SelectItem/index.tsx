import React, { ComponentPropsWithoutRef, ReactNode } from "react"
import * as SelectPrimitive from "@radix-ui/react-select"
import { IconCheck } from "../../icons"
import { SelectPrimitiveItem } from "./styles"

type SelectItemProps = { children?: ReactNode } & ComponentPropsWithoutRef<typeof SelectPrimitive.Item>

export const SelectItem = React.forwardRef<HTMLDivElement, SelectItemProps>(({ children, ...props }, forwardedRef) => {
  return (
    <SelectPrimitiveItem {...props} ref={forwardedRef}>
      <SelectPrimitive.ItemText>{children}</SelectPrimitive.ItemText>
      <SelectPrimitive.ItemIndicator>
        <IconCheck />
      </SelectPrimitive.ItemIndicator>
    </SelectPrimitiveItem>
  )
})
