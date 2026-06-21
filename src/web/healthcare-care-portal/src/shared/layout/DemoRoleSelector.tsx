import { useSecurity, DemoRole } from "../../core/security/SecurityContext";

const roles: DemoRole[] = ["CareCoordinator", "Provider", "Patient", "Admin", "Auditor"];

export function DemoRoleSelector() {
  const { role, setRole } = useSecurity();

  return (
    <div className="flex items-center space-x-2 text-sm bg-gray-100 px-3 py-1.5 rounded-full border border-gray-200">
      <span className="text-gray-500 font-medium whitespace-nowrap">Demo Role:</span>
      <select
        value={role}
        onChange={(e) => setRole(e.target.value as DemoRole)}
        className="bg-transparent text-gray-900 font-semibold focus:outline-none focus:ring-0 border-none p-0 cursor-pointer"
      >
        {roles.map(r => (
          <option key={r} value={r}>{r}</option>
        ))}
      </select>
    </div>
  );
}
