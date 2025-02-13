import { ComponentPropsWithoutRef } from "react"
import * as AccordionPrimitive from "@radix-ui/react-accordion"
import { AccordionPrimitiveItem } from "./styles"

type AccordionItemProps = ComponentPropsWithoutRef<typeof AccordionPrimitive.Item>

export function AccordionItem({ children, ...props }: AccordionItemProps) {
  return <AccordionPrimitiveItem {...props}>{children}</AccordionPrimitiveItem>
}
