import './Modal.css';
import React, { useState } from 'react';
import { Modal, Form, Button } from 'rsuite';


const AddEditReportModal: React.FC<{ show: boolean, report: any, onSave: (report: any) => void, onClose: () => void }> = ({ show, report, onSave, onClose }) => {
    const [formValue, setFormValue] = useState<any>(report || {
        name: '',
        author: '',
        dateCreated: new Date().toLocaleDateString(),
        dataSource: '',
        sqlQuery: ''
    });

    const handleChange = (value: any) => {
        setFormValue(value);
    };

    const handleSubmit = () => {
        onSave(formValue);
    };

    return (
        <Modal open={show} onClose={onClose}>
            <Modal.Header>
                <Modal.Title>{report ? '������������� �����' : '�������� �����'}</Modal.Title>
            </Modal.Header>
            <Modal.Body>
                <Form fluid>
                    <Form.Group>
                        <Form.ControlLabel>������������</Form.ControlLabel>
                        <Form.Control name="name" value={formValue.name} onChange={handleChange} />
                    </Form.Group>
                    <Form.Group>
                        <Form.ControlLabel>�����</Form.ControlLabel>
                        <Form.Control name="author" value={formValue.author} onChange={handleChange} />
                    </Form.Group>
                    <Form.Group>
                        <Form.ControlLabel>�������� ������</Form.ControlLabel>
                        <Form.Control name="dataSource" value={formValue.dataSource} onChange={handleChange} />
                    </Form.Group>
                    <Form.Group>
                        <Form.ControlLabel>SQL ������</Form.ControlLabel>
                        <Form.Control
                            name="sqlQuery"
                            componentClass="textarea"
                            rows={5}
                            value={formValue.sqlQuery}
                            onChange={handleChange}
                        />
                    </Form.Group>
                </Form>
            </Modal.Body>
            <Modal.Footer>
                <Button.Toolbar>
                    <Button onClick={handleSubmit} appearance="primary">
                        ���������
                    </Button>
                    <Button onClick={onClose} appearance="subtle">
                        ������
                    </Button>
                </Button.Toolbar>
            </Modal.Footer>
        </Modal>
    );
};
export default AddEditModalWindow;