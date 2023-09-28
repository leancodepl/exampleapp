import { FormattedMessage } from "react-intl";
import { useIsSignedIn } from "../../_hooks/useIsSignedIn";
import { SignUp } from "../../components/auth/SignUp";

export function SignUpPage() {
    const isSignedIn = useIsSignedIn();

    return isSignedIn ? <FormattedMessage defaultMessage="Already logged in" /> : <SignUp />;
}
