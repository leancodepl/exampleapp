import { Outlet, useLocation } from "react-router"
import { returnToParameterName } from "@leancodepl/kratos"
import { useIsLoggedIn } from "../../../_hooks/useIsLoggedIn"
import { loginRoute } from "../../../kratosRoutes"
import { Redirect } from "../../common/Redirect"

export function LoggedInGuard() {
    const isLoggedIn = useIsLoggedIn()
    const { pathname, search } = useLocation()

    if (isLoggedIn === false) {
        const params = new URLSearchParams({ [returnToParameterName]: `${pathname}${search}` })

        const redirectUrl = `${loginRoute}?${params}`

        return <Redirect path={redirectUrl} />
    }

    if (isLoggedIn === undefined) {
        return null
    }

    return <Outlet />
}
