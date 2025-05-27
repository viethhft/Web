import { Component, EventEmitter, Input, OnInit, Output } from "@angular/core"
import { FormArray, FormBuilder, FormGroup } from "@angular/forms";
import { UserService } from "../../../../../services/user/user.service";
import { UpdateRoleUserDto } from "../../../../../services/user/user.dtos";
import { ToastrService } from "ngx-toastr";

interface RoleItem {
    name: string;
    value: string;
    checked: boolean;
}

@Component({
    selector: 'app-edit-role',
    templateUrl: './edit-role.component.html',
    styleUrls: ['./edit-role.component.scss']
})
export class EditRoleComponent implements OnInit {
    @Input() data: any;
    @Output() close = new EventEmitter<void>();
    form: FormGroup;

    itemList: RoleItem[] = [];
    dataSendUpdate: UpdateRoleUserDto = {
        idUser: '',
        listIdRole: [],
    };
    constructor(private fb: FormBuilder, private userService: UserService,
        private toastrService: ToastrService
    ) {
        this.form = this.fb.group({
            checkboxes: this.fb.array([])
        });
    }

    ngOnInit() {
        let currentRole: string[] = this.data.roles.split(',').map((r: string) => r.trim());

        this.userService.getListRole().subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.itemList = response.data.map(role => {
                        const isChecked = currentRole.includes(role.name);
                        // Push control
                        this.checkboxes.push(this.fb.control(isChecked));
                        return {
                            name: role.name,
                            value: role.id,
                            checked: isChecked
                        };
                    });
                }
            }
            , error: (error) => {
                console.error("Error loading role permissions", error);
            }
        });
    }

    get checkboxes(): FormArray {
        return this.form.get('checkboxes') as FormArray;
    }

    saveUpdateRole() {
        this.dataSendUpdate.idUser = this.data.id;
        this.dataSendUpdate.listIdRole = this.itemList.filter(item => item.checked).map(item => item.value);
        this.userService.updateRole(this.dataSendUpdate).subscribe({
            next: (response) => {
                if (response.isSuccess) {
                    this.toastrService.success(response.message, "Thông báo");
                    this.close.emit();
                } else {
                    this.toastrService.error(response.message, "Thông báo");
                }
            },
            error: (error) => {
                console.error("Error updating role", error);
            }
        });
    }
    onCheckChange(item: RoleItem): void {
        this.itemList.filter(c => c.name === item.name)[0].checked = !this.itemList.filter(c => c.name === item.name)[0].checked;
    }
}
