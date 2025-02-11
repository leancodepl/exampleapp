import { ReactNode } from "@tanstack/react-router"
import { ProfileContainerContainer, ProfileContainerContent } from "./styles"

type ProfileContainerProps = {
  children?: ReactNode
}

export function ProfileContainer({ children }: ProfileContainerProps) {
  return (
    <ProfileContainerContainer>
      <ProfileContainerContent>{children}</ProfileContainerContent>
    </ProfileContainerContainer>
  )
}
