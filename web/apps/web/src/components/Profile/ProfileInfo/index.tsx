import { Flex, Text } from "@web/design-system"
import { IconUser01 } from "@web/design-system/icons"
import { useProfileInfo } from "../../../auth/useProfileInfo"
import { Avatar } from "./styles"

export function ProfileInfo() {
  const { firstName, lastName, email } = useProfileInfo()

  return (
    <Flex gap="medium">
      <Avatar>
        <IconUser01 size="illustration" />
      </Avatar>

      <Flex direction="column" gap="xsmall">
        <Text body bold color="default.primary">
          {firstName} {lastName}
        </Text>

        <Text body color="default.primary">
          {email}
        </Text>
      </Flex>
    </Flex>
  )
}
