import { ReactNode } from "react"
import { Flex, Select, Text } from "@web/design-system"
import { useLanguage } from "../../language/LanguageContext"
import { Logo } from "../Logo"
import IconGb from "../Profile/ProfileLanguage/icon_gb.svg?react"
import IconPl from "../Profile/ProfileLanguage/icon_pl.svg?react"
import { Content, languageSelectCss, Root } from "./styles"

type UnauthorizedLayoutProps = {
  children?: ReactNode
}

export function UnauthorizedLayout({ children }: UnauthorizedLayoutProps) {
  const { language, setLanguage } = useLanguage()

  return (
    <Root>
      <Flex justify="space-between">
        <Logo />

        <Select.Root className={languageSelectCss} value={language} onValueChange={setLanguage}>
          <Select.Content>
            <Select.Item value="pl">
              <IconPl />
              <Text body color="default.primary">
                PL
              </Text>
            </Select.Item>
            <Select.Item value="en">
              <IconGb />
              <Text body color="default.primary">
                EN
              </Text>
            </Select.Item>
          </Select.Content>
        </Select.Root>
      </Flex>

      <Content>{children}</Content>
    </Root>
  )
}
