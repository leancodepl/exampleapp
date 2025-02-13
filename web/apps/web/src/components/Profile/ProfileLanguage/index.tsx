import { FormattedMessage } from "react-intl"
import { Link } from "@tanstack/react-router"
import { Button, Flex, Radio, Text, useIsMobile } from "@web/design-system"
import { IconChevronLeft, IconMoon01, IconSun } from "@web/design-system/icons"
import { useLanguage } from "../../../language/LanguageContext"
import { Title } from "../../AppTitle"
import { useTheme } from "../../Theme"
import { ProfileContainer } from "../ProfileContainer"
import { ProfileListItem, ProfileListItems } from "../ProfileListItem"
import IconGb from "./icon_gb.svg?react"
import IconPl from "./icon_pl.svg?react"
import { profileThemeIconCss, ProfileThemeItem } from "./styles"

export function ProfileLanguage() {
  const isMobile = useIsMobile()
  const { language, setLanguage } = useLanguage()
  const { theme, setTheme } = useTheme()

  return (
    <ProfileContainer>
      {isMobile && (
        <Title>
          <Flex align="center" gap="small">
            <Button asChild leading={<IconChevronLeft />} type="text">
              <Link to="/profile" />
            </Button>
            <FormattedMessage defaultMessage="Język i wygląd" id="profile.language.title" />
          </Flex>
        </Title>
      )}

      <Flex direction="column" gap="large">
        <Flex direction="column" gap="small">
          <Text caption color="default.secondary">
            <FormattedMessage defaultMessage="Wybór języka" id="profile.language.label.languageSelect" />
          </Text>

          <ProfileListItems as={Radio.Root} value={language} onValueChange={setLanguage}>
            <ProfileListItem asChild as={Radio.Item} value="pl">
              <Flex align="center" gap="xsmall">
                <IconPl />
                Polski
              </Flex>
            </ProfileListItem>

            <ProfileListItem asChild as={Radio.Item} value="en">
              <Flex align="center" gap="xsmall">
                <IconGb />
                English
              </Flex>
            </ProfileListItem>
          </ProfileListItems>
        </Flex>

        <Flex direction="column" gap="small">
          <Text caption color="default.secondary">
            <FormattedMessage defaultMessage="Wybór motywu" id="profile.language.label.themeSelect" />
          </Text>

          <ProfileListItems as={Radio.Root} value={theme} onValueChange={setTheme}>
            <ProfileListItem asChild as={Radio.Item} value="light">
              <ProfileThemeItem>
                <FormattedMessage defaultMessage="Jasny" id="profile.language.theme.light" />

                <IconSun className={profileThemeIconCss} />
              </ProfileThemeItem>
            </ProfileListItem>

            <ProfileListItem asChild as={Radio.Item} value="dark">
              <ProfileThemeItem>
                <FormattedMessage defaultMessage="Ciemny" id="profile.language.theme.dark" />

                <IconMoon01 className={profileThemeIconCss} />
              </ProfileThemeItem>
            </ProfileListItem>

            <ProfileListItem as={Radio.Item} value="system">
              <FormattedMessage defaultMessage="System" id="profile.language.theme.system" />
            </ProfileListItem>
          </ProfileListItems>
        </Flex>
      </Flex>
    </ProfileContainer>
  )
}
