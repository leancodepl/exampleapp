import { BaseSessionManager } from "@leancodepl/kratos";
import { map } from "rxjs";
import { environment } from "../environments/environment";
import { signInRoute } from "../routes";

class SessionManager extends BaseSessionManager {
    email$ = this.identity$.pipe(
        map(identity => {
            const traits: unknown = identity?.traits;

            return traits && typeof traits === "object" && "email" in traits && typeof traits.email === "string"
                ? traits.email
                : undefined;
        }),
    );
}

export const sessionManager = new SessionManager(environment.authUrl, signInRoute);
