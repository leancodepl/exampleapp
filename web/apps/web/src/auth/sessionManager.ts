import { map } from "rxjs"
import { BaseSessionManager } from "@leancodepl/kratos"
import { environment } from "../environments/environment"

export class SessionManager extends BaseSessionManager {
  metadata$ = this.identity$.pipe(
    map(identity => {
      const metadata: unknown = identity?.metadata_public

      return metadata && typeof metadata === "object" ? metadata : undefined
    }),
  )

  traits$ = this.identity$.pipe(
    map(identity => {
      const traits: unknown = identity?.traits

      return traits && typeof traits === "object" ? traits : undefined
    }),
  )

  email$ = this.traits$.pipe(
    map(traits => {
      return traits && "email" in traits && typeof traits.email === "string" ? traits.email : undefined
    }),
  )

  firstName$ = this.traits$.pipe(
    map(traits => {
      return traits && "first_name" in traits && typeof traits.first_name === "string" ? traits.first_name : undefined
    }),
  )

  lastName$ = this.traits$.pipe(
    map(traits => {
      return traits && "last_name" in traits && typeof traits.last_name === "string" ? traits.last_name : undefined
    }),
  )
}

export const sessionManager = new SessionManager(environment.authUrl, "/login")
