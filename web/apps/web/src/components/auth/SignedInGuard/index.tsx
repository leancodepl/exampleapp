import { returnToParameterName } from "@leancodepl/kratos";
import { Outlet, useLocation } from "react-router";
import { useIsSignedIn } from "../../../_hooks/useIsSignedIn";
import { signInRoute } from "../../../routes";
import { Redirect } from "../../common/Redirect";

export default function SignedInGuard() {
    const isSignedIn = useIsSignedIn();
    const { pathname, search } = useLocation();

    if (isSignedIn === false) {
        const params = new URLSearchParams({ [returnToParameterName]: `${pathname}${search}` });

        const redirectUrl = `${signInRoute}?${params}`;

        return <Redirect path={redirectUrl} />;
    }

    if (isSignedIn === undefined) {
        return null;
    }

    return <Outlet />;
}
