import { ComponentPropsWithoutRef } from "react"
import * as AccordionPrimitive from "@radix-ui/react-accordion"
import { IconChevronDown } from "../../icons"
import { Text } from "../../Text"
import { AccordionPrimitiveHeader, AccordionPrimitiveIndicator, AccordionPrimitiveTrigger } from "./styles"

type AccordionHeaderProps = ComponentPropsWithoutRef<typeof AccordionPrimitive.Item>

export function AccordionHeader({ children, ...props }: AccordionHeaderProps) {
  return (
    <AccordionPrimitiveTrigger>
      <Text asChild body color="default.primary">
        <AccordionPrimitiveHeader {...props}>{children}</AccordionPrimitiveHeader>
      </Text>

      <AccordionPrimitiveIndicator>
        <IconChevronDown />
      </AccordionPrimitiveIndicator>
    </AccordionPrimitiveTrigger>
  )
}
