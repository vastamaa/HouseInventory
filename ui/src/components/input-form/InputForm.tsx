import React from "react";
import Input, { IInputProps } from "../input/Input";
import { Element, IButtonField, IInputField } from "../../configs/form-config";
import Button, { IButtonProps } from "../button/Button";

interface IInputFormProps {
    configurations: Element[];
}

const isInputField = (element: Element): element is IInputField => element.fieldType === 'input'
const isButton = (element: Element): element is IButtonField => element.fieldType === 'button'

const InputForm = (props: IInputFormProps): JSX.Element => {
    const getElement = (element: Element): JSX.Element => {
        if (isInputField(element)) {
            return (
                <>
                    <Input {...mapToInputProps(element)} />
                    <br />
                </>
            )
        }

        if (isButton(element)) {
            return (
                <>
                    <Button {...mapToButtonProps(element)} />
                    <br />
                </>
            )
        }

        console.error('Unknown element type:', element);
        return null;
    }

    return (
        <div>
            {props.configurations.map((configuration, index) => {
                return (
                    <div key={index}>
                        {getElement(configuration)}
                    </div>
                )
            })}
        </div>
    )
}

const mapToInputProps = (props: IInputField): IInputProps => {
    return {
        defaultText: props.defaultText,
        onHandleInputChange: props.onChange,
        value: props.value,
        name: props.name
    }
}

const mapToButtonProps = (props: IButtonField): IButtonProps => {
    return {
        value: props.value,
        onHandleButtonClick: props.onClick
    }
}

export default InputForm;