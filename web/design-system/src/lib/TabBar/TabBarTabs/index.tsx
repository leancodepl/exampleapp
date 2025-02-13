import { ReactNode } from "@tanstack/react-router"
import { dataFill } from "../attributes"
import { TabBarTabsContainer } from "./styles"

export type TabBarTabsProps = {
  children?: ReactNode
  fill?: boolean
}

export function TabBarTabs({ children, fill }: TabBarTabsProps) {
  return <TabBarTabsContainer {...dataFill(fill ? "" : undefined)}>{children}</TabBarTabsContainer>
}
