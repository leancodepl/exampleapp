import { createFileRoute } from "@tanstack/react-router"
import { ProfilePassword } from "../../../components/Profile/ProfilePassword"

export const Route = createFileRoute("/_authorized/profile/password")({
  component: ProfilePassword,
})
