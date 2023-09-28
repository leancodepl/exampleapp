import { CustomUiMessage } from "@leancodepl/kratos";
import { UiText } from "@ory/kratos-client";
import { getMessages } from "./getMessages";
import { message } from "../../../_utils/globalApp";

export function displayGlobalMessages(messages: UiText[], customUiMessage?: CustomUiMessage) {
    const { error, info, success } = getMessages({ messages, attributes: undefined, customUiMessage });

    if (error) message.error(error);
    if (info) message.info(info);
    if (success) message.success(success);
}
