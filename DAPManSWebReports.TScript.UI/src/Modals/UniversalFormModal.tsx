import React, { useEffect, useState } from 'react';
import { Button, ButtonToolbar, Form, Modal } from 'rsuite';

// Универсальное модальное окно для редактирования/добавления сущностей
const UniversalFormModal:React.FC<{ show: boolean, entity: any, onSave: (entity: any) => void, onClose: () => void }> = ({ show, entity, fields, onSave, onClose }) => {
    const [formValue, setFormValue] = useState({});

    useEffect(() => {
        // Инициализация значений формы на основе переданных entity
        const initialFormValue = fields.reduce((acc, field) => {
            acc[field.name] = entity?.[field.name] || '';
            return acc;
        }, {});
        setFormValue(initialFormValue);
    }, [entity, fields]);

    const handleChange = (value, name) => {
        setFormValue(prevState => ({
            ...prevState,
            [name]: value
        }));
    };

    const handleSubmit = () => {
        onSave(formValue);
    };

    return (
        <Modal open={show} onClose={onClose}>
            <Modal.Header>
                <Modal.Title>{entity ? 'Редактировать' : 'Добавить'} сущность</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form fluid>
                    {fields.map(field => (
                        <Form.Group key={field.name}>
                            <Form.ControlLabel>{field.label}</Form.ControlLabel>
                            <Form.Control
                                name={field.name}
                                type={field.type || "text"}
                                value={formValue[field.name]}
                                componentClass={field.componentClass || "input"}
                                rows={field.rows || undefined}
                                onChange={(value) => handleChange(value, field.name)}
                            />
                        </Form.Group>
                    ))}
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <ButtonToolbar>
                    <Button onClick={handleSubmit} appearance="primary">
                        Сохранить
                    </Button>
                    <Button onClick={onClose} appearance="subtle">
                        Отмена
                    </Button>
                </ButtonToolbar>
            </Modal.Footer>
        </Modal>
    );
};

export default UniversalFormModal;