import { ComponentPropsWithRef } from "react"
import { Slot, Slottable } from "@radix-ui/react-slot"
import * as TabsPrimitive from "@radix-ui/react-tabs"
import { ReactNode } from "@tanstack/react-router"
import { AnimatePresence } from "motion/react"
import { TabBarTabBar, TabBarTabContainer } from "./styles"

type TabBarTabProps = {
  value: string
  asChild?: boolean
  children?: ReactNode
}

export function TabBarTab({ children, asChild, value }: TabBarTabProps) {
  return (
    <TabsPrimitive.Trigger asChild value={value}>
      <TabBarTabInternal asChild={asChild}>{children}</TabBarTabInternal>
    </TabsPrimitive.Trigger>
  )
}

type TabBarTabInternalProps = {
  "data-state"?: "active" | "inactive"
  asChild?: boolean
} & ComponentPropsWithRef<"button">

function TabBarTabInternal({ "data-state": dataState, asChild, children, ...props }: TabBarTabInternalProps) {
  const Component = asChild ? Slot : "button"

  return (
    <TabBarTabContainer as={Component} {...props} data-state={dataState}>
      <Slottable>{children}</Slottable>

      <AnimatePresence>{dataState === "active" && <TabBarTabBar layoutId="tabbar" />}</AnimatePresence>
    </TabBarTabContainer>
  )
}
