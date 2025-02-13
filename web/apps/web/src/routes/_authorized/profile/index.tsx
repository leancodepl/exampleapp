import { createFileRoute } from "@tanstack/react-router"
import { ProfileMenu } from "../../../components/Profile/ProfileMenu"

export const Route = createFileRoute("/_authorized/profile/")({
  component: ProfileMenu,
})
