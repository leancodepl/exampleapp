import { ReactNode, useState } from "react"
import { FormattedMessage } from "react-intl"
import { Input, Modal } from "antd"

type AddByNameModalProps = {
    title: ReactNode
    onAdd: (name: string) => void
    onClose: () => void
    isOpen: boolean
    isAdding?: boolean
}

export function AddByNameModal({ title, onAdd, onClose, isOpen, isAdding }: AddByNameModalProps) {
    const [name, setName] = useState("")

    return (
        <Modal
            confirmLoading={isAdding}
            open={isOpen}
            title={title}
            onCancel={onClose}
            onOk={() => {
                onAdd(name)
                onClose()
            }}>
            <FormattedMessage defaultMessage="Name" />
            <Input value={name} onChange={e => setName(e.target.value)} />
        </Modal>
    )
}
