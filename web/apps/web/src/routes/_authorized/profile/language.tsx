import { createFileRoute } from "@tanstack/react-router"
import { ProfileLanguage } from "../../../components/Profile/ProfileLanguage"

export const Route = createFileRoute("/_authorized/profile/language")({
  component: ProfileLanguage,
})
